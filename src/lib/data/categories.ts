/**
 * Categories Data Access Layer
 * This is the ONLY module your components should import for category data.
 */
import { fetchCategories } from '@/lib/api/client';
import { fallbackCategories, getCategoryDisplayName as getFallbackDisplayName } from './fallback';

/**
 * Get all category slugs.
 * Always returns an array - either from API or fallback.
 */
export async function getAllCategories(): Promise<string[]> {
  try {
    const apiCategories = await fetchCategories();
    if (Array.isArray(apiCategories) && apiCategories.length > 0) {
      return apiCategories;
    }
    console.warn('API returned no categories, using fallback.');
    return fallbackCategories();
  } catch (err) {
    console.error('Categories API error, using fallback:', err);
    return fallbackCategories();
  }
}

/**
 * Get user-friendly display name for a category slug.
 * e.g., "props-cosplay" -> "Props & Cosplay"
 */
export async function getDisplayNameFromSlug(slug: string): Promise<string> {
  return getFallbackDisplayName(slug);
}