import build from 'next/dist/build'
import {
    ModelsResponse,
    ModelItem,
    ModelsQueryParams,
    ApiError
} from './types'
import { URLSearchParams } from 'next/dist/compiled/@edge-runtime/primitives/url'

const BACKEND_URL = (
    process.env.BACKEND_URL ?? 
    process.env.NEXT_PUBLIC_API_URL ?? 
    'http://localhost:5000'
).replace(/\/$/, '')




export function buildApiUrl(path: string): string {
    const cleanPath = path.startsWith('/') ? path : `/${path}`
    return `${BACKEND_URL}${cleanPath}`
}

export function getImageUrl(modelId: number): string {
    return buildApiUrl(`/3dmodels/${modelId}/thumbnail`)
}

function buildQueryString(params: Record<string, unknown>): string {
    const searchParams = new URLSearchParams()
    for (const [key, value] of Object.entries(params)) {
        if (value !== undefined && value !== null && value !== ''){
            searchParams.append(key, String(value))
        }
    }
    const queryString = searchParams.toString()
    return queryString ? `?${queryString}` : ''
}

async function apiFetch<T>(url: string, options?: RequestInit): Promise<T> {
    const startTime = Date.now()

    try {
        console.log(`Api Request: ${options?.method ?? 'GET'} ${url}`);

        const res = await fetch(url, {...options,
            headers: {'Content-Type': 'application/json', ...options?.headers },
            cache: options?.cache ?? 'no-store',
        })
        const elapsed = Date.now() - startTime

        if (!res.ok){
            const errorBody = await res.text()
            console.error(`API error (${res.status} in ${elapsed}ms): ${url}`, errorBody)
            throw new ApiError(`Api request failed: ${res.statusText}`,
            res.status,
            errorBody
            )
        }
        const data = await res.json() as T
        console.log(`Api Success (200 in ${elapsed}ms): ${url}`)
        return data
    }
    catch (error){
        console.error(`Api Netword for ${url}:`, error)
        if (error instanceof ApiError){
            throw error
        }
        throw new ApiError('An unexpected error has occured',
            undefined,
            error
        )
    }
}