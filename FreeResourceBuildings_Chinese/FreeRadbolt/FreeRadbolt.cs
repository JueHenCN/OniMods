﻿// Decompiled with JetBrains decompiler
// Type: FreeRadbolt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C7E9CCB-4AA8-44E2-BE45-38990AABF98E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FreeResourceBuildingsPatches;
using KSerialization;
using STRINGS;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class FreeRadbolt :
  StateMachineComponent<FreeRadbolt.StatesInstance>,
  IHighEnergyParticleDirection,
  IProgressBarSideScreen,
  ISingleSliderControl,
  ISliderControl
{
	[MyCmpReq]
	private HighEnergyParticleStorage particleStorage;
	[MyCmpGet]
	private Operational operational;
	private float recentPerSecondConsumptionRate;
	public int minSlider;
	public int maxSlider;
	[Serialize]
	private EightDirection _direction;
	public float minLaunchInterval;
	public float radiationSampleRate;
	[Serialize]
	public float particleThreshold = Mod.Options.defaultRadthreshold;
	[Serialize]
	public float amountPerCycle = Mod.Options.defaultRadGeneration;
	private EightDirectionController directionController;
	private float launcherTimer;
	private float radiationSampleTimer;
	private MeterController particleController;
	private bool particleVisualPlaying;
	private MeterController progressMeterController;
	[Serialize]
	public Ref<HighEnergyParticlePort> capturedByRef = new Ref<HighEnergyParticlePort>();
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;
	private static readonly EventSystem.IntraObjectHandler<FreeRadbolt> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<FreeRadbolt>((System.Action<FreeRadbolt, object>)((component, data) => component.OnCopySettings(data)));

	public float PredictedPerCycleConsumptionRate => (float)Mathf.FloorToInt((float)((double)this.recentPerSecondConsumptionRate * 0.100000001490116 * 600.0));
	public static StatusItem CollectingHEP;
	public EightDirection Direction
	{
		get => this._direction;
		set
		{
			this._direction = value;
			if (this.directionController == null)
				return;
			this.directionController.SetRotation((float)(45 * EightDirectionUtil.GetDirectionIndex(this._direction)));
			this.directionController.controller.enabled = false;
			this.directionController.controller.enabled = true;
		}
	}

	private StatusItem CreateStatusItem(
	 string id,
	 string prefix,
	 string icon,
	 StatusItem.IconType icon_type,
	 NotificationType notification_type,
	 bool allow_multiples,
	 HashedString render_overlay,
	 bool showWorldIcon = true,
	 int status_overlays = 129022)
	{
		if (CollectingHEP == null)
		{
			return Db.Get().BuildingStatusItems.Add(new StatusItem(id, prefix, icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays));
		}
		return CollectingHEP;
	}
	private void OnCopySettings(object data)
	{
		FreeRadbolt component = ((GameObject)data).GetComponent<FreeRadbolt>();
		if (!((UnityEngine.Object)component != (UnityEngine.Object)null))
			return;
		this.Direction = component.Direction;
		this.particleThreshold = component.particleThreshold;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();

		CollectingHEP = CreateStatusItem("CollectingFreeHEP", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.Neutral, allow_multiples: false, OverlayModes.Radiation.ID, showWorldIcon: false);
		CollectingHEP.resolveStringCallback = ((string str, object data) => str.Replace("{x}", ((FreeRadbolt)data).PredictedPerCycleConsumptionRate.ToString()));

		this.Subscribe<FreeRadbolt>(-905833192, FreeRadbolt.OnCopySettingsDelegate);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi.StartSM();
		this.directionController = new EightDirectionController((KAnimControllerBase)this.GetComponent<KBatchedAnimController>(), "redirector_target", "redirect", EightDirectionController.Offset.Infront);
		this.Direction = this.Direction;
		this.particleController = new MeterController((KAnimControllerBase)this.GetComponent<KBatchedAnimController>(), "orb_target", "orb_off", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.particleController.gameObject.AddOrGet<LoopingSounds>();
		this.progressMeterController = new MeterController((KAnimControllerBase)this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());

	}

	public float GetProgressBarMaxValue() => this.particleThreshold;

	public float GetProgressBarFillPercentage() => this.particleStorage.Particles / this.particleThreshold;

	public string GetProgressBarTitleLabel() => (string)UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.PROGRESS_BAR_LABEL;

	public string GetProgressBarLabel()
	{
		int num = Mathf.FloorToInt(this.particleStorage.Particles);
		string str1 = num.ToString();
		num = Mathf.FloorToInt(this.particleThreshold);
		string str2 = num.ToString();
		return str1 + "/" + str2;
	}

	public string GetProgressBarTooltip() => (string)UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.PROGRESS_BAR_TOOLTIP;

	public void DoConsumeParticlesWhileDisabled(float dt)
	{
		//double num = (double)this.particleStorage.ConsumeAndGet(dt * 1f);
		this.progressMeterController.SetPositionPercent(this.GetProgressBarFillPercentage());
	}

	public void LauncherUpdate(float dt)
	{
		this.radiationSampleTimer += dt;
		if ((double)this.radiationSampleTimer >= (double)this.radiationSampleRate)
		{
			this.radiationSampleTimer -= this.radiationSampleRate;
			int cell = Grid.PosToCell((KMonoBehaviour)this);

			if (amountPerCycle > 0 && (double)this.particleStorage.RemainingCapacity() > 0.0)
			{
				this.smi.sm.isAbsorbingRadiation.Set(true, this.smi);
				this.recentPerSecondConsumptionRate = amountPerCycle * 10 / 600;
				double num2 = (double)this.particleStorage.Store((float)((double)this.recentPerSecondConsumptionRate * (double)this.radiationSampleRate * 0.100000001490116));
			}
			else
			{
				this.recentPerSecondConsumptionRate = 0.0f;
				this.smi.sm.isAbsorbingRadiation.Set(false, this.smi);
			}
		}
		this.progressMeterController.SetPositionPercent(this.GetProgressBarFillPercentage());
		if (!this.particleVisualPlaying && (double)this.particleStorage.Particles > (double)this.particleThreshold / 2.0)
		{
			this.particleController.meterController.Play((HashedString)"orb_pre");
			this.particleController.meterController.Queue((HashedString)"orb_idle", KAnim.PlayMode.Loop);
			this.particleVisualPlaying = true;
		}
		this.launcherTimer += dt;
		if ((double)this.launcherTimer < (double)this.minLaunchInterval || (double)this.particleStorage.Particles < (double)this.particleThreshold)
			return;
		this.launcherTimer = 0.0f;
		int particleOutputCell = this.GetComponent<Building>().GetHighEnergyParticleOutputCell();
		GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab((Tag)"HighEnergyParticle"), Grid.CellToPosCCC(particleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2);
		gameObject.SetActive(true);
		if (!((UnityEngine.Object)gameObject != (UnityEngine.Object)null))
			return;
		HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
		component.payload = this.particleStorage.ConsumeAndGet(this.particleThreshold);
		component.SetDirection(this.Direction);
		this.directionController.PlayAnim("redirect_send");
		this.directionController.controller.Queue((HashedString)"redirect");
		this.particleController.meterController.Play((HashedString)"orb_send");
		this.particleController.meterController.Queue((HashedString)"orb_off");
		this.particleVisualPlaying = false;
	}

	public string SliderTitleKey => "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TITLE";

	public string SliderUnits => (string)UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;

	public int SliderDecimalPlaces(int index) => 0;

	public float GetSliderMin(int index) =>  (float)this.minSlider;

	public float GetSliderMax(int index) => (float)this.maxSlider;

	public float GetSliderValue(int index) =>  this.particleThreshold;

	public void SetSliderValue(float value, int index) =>this.particleThreshold = value;
	

	public string GetSliderTooltipKey(int index) => "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP";

	string ISliderControl.GetSliderTooltip(int index) => string.Format((string)Strings.Get("STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP"), (object)this.particleThreshold);

	public class StatesInstance :
	  GameStateMachine<FreeRadbolt.States, FreeRadbolt.StatesInstance, FreeRadbolt, object>.GameInstance
	{
		public StatesInstance(FreeRadbolt smi)
		  : base(smi)
		{
		}
	}

	public class States :
	  GameStateMachine<FreeRadbolt.States, FreeRadbolt.StatesInstance, FreeRadbolt>
	{
		public StateMachine<FreeRadbolt.States, FreeRadbolt.StatesInstance, FreeRadbolt, object>.BoolParameter isAbsorbingRadiation;
		public FreeRadbolt.States.ReadyStates ready;
		public FreeRadbolt.States.InoperationalStates inoperational;

		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = (StateMachine.BaseState)this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, (GameStateMachine<FreeRadbolt.States, FreeRadbolt.StatesInstance, FreeRadbolt, object>.State)this.ready).DefaultState(this.inoperational.empty);
			this.inoperational.empty.EventTransition(GameHashes.OnParticleStorageChanged, this.inoperational.losing, (StateMachine<FreeRadbolt.States, FreeRadbolt.StatesInstance, FreeRadbolt, object>.Transition.ConditionCallback)(smi => !smi.GetComponent<HighEnergyParticleStorage>().IsEmpty()));
			//this.inoperational.losing.ToggleStatusItem(Db.Get().BuildingStatusItems.LosingRadbolts).Update((System.Action<FreeRadbolt.StatesInstance, float>)((smi, dt) => smi.master.DoConsumeParticlesWhileDisabled(dt)), UpdateRate.SIM_1000ms).EventTransition(GameHashes.OnParticleStorageChanged, this.inoperational.empty, (StateMachine<FreeRadbolt.States, FreeRadbolt.StatesInstance, FreeRadbolt, object>.Transition.ConditionCallback)(smi => smi.GetComponent<HighEnergyParticleStorage>().IsEmpty()));
			this.ready.TagTransition(GameTags.Operational, (GameStateMachine<FreeRadbolt.States, FreeRadbolt.StatesInstance, FreeRadbolt, object>.State)this.inoperational, true).DefaultState(this.ready.idle).Update((System.Action<FreeRadbolt.StatesInstance, float>)((smi, dt) => smi.master.LauncherUpdate(dt)), UpdateRate.SIM_EVERY_TICK);
			this.ready.idle.ParamTransition<bool>((StateMachine<FreeRadbolt.States, FreeRadbolt.StatesInstance, FreeRadbolt, object>.Parameter<bool>)this.isAbsorbingRadiation, this.ready.absorbing, GameStateMachine<FreeRadbolt.States, FreeRadbolt.StatesInstance, FreeRadbolt, object>.IsTrue).PlayAnim("on");
			this.ready.absorbing.Enter("SetActive(true)", (StateMachine<FreeRadbolt.States, FreeRadbolt.StatesInstance, FreeRadbolt, object>.State.Callback)(smi => smi.master.operational.SetActive(true))).Exit("SetActive(false)", (StateMachine<FreeRadbolt.States, FreeRadbolt.StatesInstance, FreeRadbolt, object>.State.Callback)(smi => smi.master.operational.SetActive(false))).ParamTransition<bool>((StateMachine<FreeRadbolt.States, FreeRadbolt.StatesInstance, FreeRadbolt, object>.Parameter<bool>)this.isAbsorbingRadiation, this.ready.idle, GameStateMachine<FreeRadbolt.States, FreeRadbolt.StatesInstance, FreeRadbolt, object>.IsFalse).ToggleStatusItem(CollectingHEP, (Func<FreeRadbolt.StatesInstance, object>)(smi => (object)smi.master)).PlayAnim("working_loop", KAnim.PlayMode.Loop);
		}

		public class InoperationalStates :
		  GameStateMachine<FreeRadbolt.States, FreeRadbolt.StatesInstance, FreeRadbolt, object>.State
		{
			public GameStateMachine<FreeRadbolt.States, FreeRadbolt.StatesInstance, FreeRadbolt, object>.State empty;
			public GameStateMachine<FreeRadbolt.States, FreeRadbolt.StatesInstance, FreeRadbolt, object>.State losing;
		}

		public class ReadyStates :
		  GameStateMachine<FreeRadbolt.States, FreeRadbolt.StatesInstance, FreeRadbolt, object>.State
		{
			public GameStateMachine<FreeRadbolt.States, FreeRadbolt.StatesInstance, FreeRadbolt, object>.State idle;
			public GameStateMachine<FreeRadbolt.States, FreeRadbolt.StatesInstance, FreeRadbolt, object>.State absorbing;
		}
	}
}
