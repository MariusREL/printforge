export interface ModelsResponse {
    totalCount: number;
    items: ModelItem[];
}

export interface ModelItem {
    id: number;
    name: string;
    description: string;
    likes: number;
    image: string;
    category: string;
    dateAdded: string;
}

export interface ModelsQueryParams {
    category?: string;
    q?: string;
    skip?: number;
    take?: number;
    sort?: 'likes' | 'date' | 'name'
    order?: 'asc' |'desc'
}

export class ApiError extends Error {
    constructor(
        message: string,
        public status?: number,
        public body?: unknown
    ) {
        super(message)
        this.name = 'ApiError'
    }
}