/**
 * Models Data Access Layer
 * This is the ONLY module your components should import for model data.
 */
import type { ModelsResponse, ModelItem, ModelsQueryParams } from '@/lib/api/types';
import { ApiError } from '@/lib/api/types';
import { fetchModels, fetchModelById } from '@/lib/api/client';
import { fallbackModels, fallbackModelById } from './fallback';

/**
 * Get all models with optional filtering/sorting.
 * Always returns data - either from API or fallback.
 */
export async function getAllModels(
  params?: ModelsQueryParams
): Promise<ModelsResponse> {
  try {
    const response = await fetchModels(params);
    return response;
  } catch (error) {
    console.warn('⚠️ API unavailable, using fallback models.', error);
    return fallbackModels(params);
  }
}

/**
 * Get a single model by ID.
 * Returns the model or undefined if not found anywhere.
 */
export async function getModelById(id: number): Promise<ModelItem | undefined> {
  try {
    return await fetchModelById(id);
  } catch (error) {
    if (error instanceof ApiError && error.status === 404) {
      console.log(`Model ${id} not in API, checking fallback.`);
    } else {
      console.warn(`⚠️ API error for model ${id}, using fallback.`, error);
    }
    return fallbackModelById(id);
  }
}