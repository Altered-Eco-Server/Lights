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
    public partial class OutdoorDecoLampObject : WorldObject, IRepresentsItem
    {
        public override LocString DisplayName { get { return Localizer.DoStr("A beautiful outdoor wall lamp"); } }
        public override TableTextureMode TableTexture => TableTextureMode.Metal;
        public virtual Type RepresentedItemType { get { return typeof(OutdoorDecoLampItem); } }

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
    [LocDisplayName("Deco Outdoor Lamp")]
    public partial class OutdoorDecoLampItem : WorldObjectItem<OutdoorDecoLampObject>
    {
        public override LocString DisplayDescription => Localizer.DoStr("");
        [Tooltip(7)] private LocString PowerConsumptionTooltip => Localizer.Do($"Consumes: {Text.Info(60)}w of {new ElectricPower().Name} power");
    }

    [RequiresSkill(typeof(ElectronicsSkill), 1)]
    public partial class DecoOutdoorLampLightRecipe : RecipeFamily
    {
        public DecoOutdoorLampLightRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                "DecoOutdoorLamp",  //noloc
                Localizer.DoStr("Deco Outdoor Lamp"),
                new List<IngredientElement>
                {
                    new IngredientElement(typeof(SteelBarItem), 8, typeof(ElectronicsSkill), typeof(ElectronicsLavishResourcesTalent)),
                    new IngredientElement(typeof(GlassItem), 4, typeof(ElectronicsSkill), typeof(ElectronicsLavishResourcesTalent)),
                    new IngredientElement(typeof(CopperWiringItem), 5, typeof(ElectronicsSkill), typeof(ElectronicsLavishResourcesTalent)),
                    new IngredientElement(typeof(LightBulbItem), 1, true),
                },
                new List<CraftingElement>
                {
                    new CraftingElement<OutdoorDecoLampItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 5;
            this.LaborInCalories = CreateLaborInCaloriesValue(200, typeof(ElectronicsSkill));
            this.CraftMinutes = CreateCraftTimeValue(typeof(DecoOutdoorLampLightRecipe), 4, typeof(ElectronicsSkill), typeof(ElectronicsFocusedSpeedTalent), typeof(ElectronicsParallelSpeedTalent));
            this.ModsPreInitialize();
            this.Initialize(Localizer.DoStr("Deco Outdoor Lamp"), typeof(DecoOutdoorLampLightRecipe));
            this.ModsPostInitialize();
            CraftingComponent.AddRecipe(typeof(RoboticAssemblyLineObject), this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }
}
