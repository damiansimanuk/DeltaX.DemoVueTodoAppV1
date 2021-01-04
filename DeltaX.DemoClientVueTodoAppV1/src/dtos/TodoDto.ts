export interface TodoDto {
    id: number;
    description: string;
    completed: boolean;
    created: Date;
    updated: Date;
}

export interface TodoUpdateDto {
    id: number;
    description: string;
    completed: boolean;
}

export interface TodoCreateDto {
    description: string;
    completed: boolean;
}

export interface PaginatingTodosDto {
    items: TodoDto[];
    maxResultCount: number;
    skipCount: number;
    totalCount: number;
}