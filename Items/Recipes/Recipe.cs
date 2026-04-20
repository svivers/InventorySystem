using System;
using System.Collections.Generic;

namespace Core.Items.Recipes
{
    public class Recipe
    {
        private readonly RecipeId m_id;
        private readonly ItemQuantity m_product;
        private readonly ItemQuantity[] m_ingredients;

        private Recipe(RecipeId id, ItemQuantity product, ItemQuantity[] ingredients)
        {
            m_id = id;
            m_product = product;
            m_ingredients = ingredients;
        }

        public RecipeId Id => m_id;
        public ItemQuantity Product => m_product;
        public IReadOnlyList<ItemQuantity> Ingredients => m_ingredients;

        public static RecipeBuilder Builder() => new RecipeBuilder();

        public class RecipeBuilder
        {
            private readonly List<ItemQuantity> m_ingredients;
            private RecipeId m_id;
            private ItemQuantity m_product;
            private bool m_isProductSet;

            public RecipeBuilder()
            {
                m_ingredients = new List<ItemQuantity>();
            }

            public RecipeBuilder AddIngredient(ItemQuantity ingredient)
            {
                m_ingredients.Add(ingredient);
                return this;
            }

            public RecipeBuilder SetId(RecipeId id)
            {
                m_id = id;
                return this;
            }

            public RecipeBuilder SetProduct(ItemQuantity product)
            {
                m_isProductSet = true;
                m_product = product;
                return this;
            }

            public Recipe Build()
            {
                if (!m_id.IsValid)
                    throw new InvalidOperationException($"Must assign recipe id before building");

                if (!m_isProductSet)
                    throw new InvalidOperationException($"Must assign product before building");

                return new Recipe(m_id, m_product, m_ingredients.ToArray());
            }
        }
    }
}
