namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Eco.Core.Items;
    using Eco.Gameplay.Blocks;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Auth;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Economy;
    using Eco.Gameplay.Housing;
    using Eco.Gameplay.Interactions;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Modules;
    using Eco.Gameplay.Minimap;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Property;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Gameplay.Pipes.LiquidComponents;
    using Eco.Gameplay.Pipes.Gases;
    using Eco.Gameplay.Systems.Tooltip;
    using Eco.Shared;
    using Eco.Shared.Math;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.Shared.View;
    using Eco.Shared.Items;
    using Eco.Gameplay.Pipes;
    using Eco.World.Blocks;
    using Eco.Gameplay.Housing.PropertyValues;
    using static Eco.Gameplay.Housing.PropertyValues.HomeFurnishingValue;

    [Serialized]
    [RequireComponent(typeof(OnOffComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(PowerGridComponent))]
    [RequireComponent(typeof(PowerConsumptionComponent))]
    public partial class OutdoorLampObject : WorldObject, IRepresentsItem
    {
        public override LocString DisplayName { get { return Localizer.DoStr("A standard outdoor wall lamp"); } }
        public override TableTextureMode TableTexture => TableTextureMode.Metal;
        public virtual Type RepresentedItemType { get { return typeof(OutdoorLampItem); } }

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<PowerConsumptionComponent>().Initialize(60);
            this.GetComponent<PowerGridComponent>().Initialize(10, new ElectricPower());
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Outdoor Lamp")]
    public partial class OutdoorLampItem : WorldObjectItem<OutdoorLampObject>
    {
        public override LocString DisplayDescription => Localizer.DoStr("");
        [Tooltip(7)] private LocString PowerConsumptionTooltip => Localizer.Do($"Consumes: {Text.Info(60)}w of {new ElectricPower().Name} power");
    }

    [RequiresSkill(typeof(ElectronicsSkill), 1)]
    public partial class OutdoorLampRecipe : RecipeFamily
    {
        public OutdoorLampRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                "OutdoorLamp",  //noloc
                Localizer.DoStr("Outdoor Lamp"),
                new List<IngredientElement>
                {
                    new IngredientElement(typeof(IronBarItem), 8, typeof(ElectronicsSkill), typeof(ElectronicsLavishResourcesTalent)),
                    new IngredientElement(typeof(PlasticItem), 6, typeof(ElectronicsSkill), typeof(ElectronicsLavishResourcesTalent)),
                    new IngredientElement(typeof(CopperWiringItem), 5, typeof(ElectronicsSkill), typeof(ElectronicsLavishResourcesTalent)),
                    new IngredientElement(typeof(LightBulbItem), 1, true),
                },
                new List<CraftingElement>
                {
                    new CraftingElement<OutdoorLampItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 5;
            this.LaborInCalories = CreateLaborInCaloriesValue(200, typeof(ElectronicsSkill));
            this.CraftMinutes = CreateCraftTimeValue(typeof(OutdoorLampRecipe), 4, typeof(ElectronicsSkill), typeof(ElectronicsFocusedSpeedTalent), typeof(ElectronicsParallelSpeedTalent));
            this.ModsPreInitialize();
            this.Initialize(Localizer.DoStr("Outdoor Lamp"), typeof(OutdoorLampRecipe));
            this.ModsPostInitialize();
            CraftingComponent.AddRecipe(typeof(RoboticAssemblyLineObject), this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }
}
