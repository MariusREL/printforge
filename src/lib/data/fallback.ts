import type { ModelsResponse, ModelItem } from '@/lib/api/types';
import modelsJson from '@/app/data/models.json';
import categoriesJson from '@/app/data/categories.json';

const FALLBACK_MODELS = modelsJson as ModelItem[];

export function fallbackModels(params?: {
    category?: string,
    skip?: number,
    take?: number,
}): ModelsResponse {
    let items = [...FALLBACK_MODELS]

    if (params?.category){
        items = items.filter(
            (model) => model.category.toLowerCase() === params.category!.toLowerCase()
        )
    }
    const totalCount = items.length;

    const skip = params?.skip ?? 0;
    const take = params?.take ?? items.length
    items = items.slice(skip, skip + take);

    return { totalCount, items}
}

export function fallbackModelById(id: number): ModelItem | undefined {
  return FALLBACK_MODELS.find((m) => m.id === id);
}

/**
 * Fallback for fetchCategories() - returns array of category slugs
 */
export function fallbackCategories(): string[] {
  // Extract category names from the static JSON
  const categories = categoriesJson as Array<{ slug: string; name: string }>;
  return categories.map((c) => c.slug);
}

/**
 * Get display name for a category slug (e.g., "props-cosplay" -> "Props & Cosplay")
 */
export function getCategoryDisplayName(slug: string): string {
  const categories = categoriesJson as Array<{ slug: string; name: string }>;
  const found = categories.find((c) => c.slug === slug);
  return found?.name ?? slug;
}