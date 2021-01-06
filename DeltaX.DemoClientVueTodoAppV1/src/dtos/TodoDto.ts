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

export interface TodoDtoDataTrackerResultDto {
    items?: { updated: Date, reason: number, item: TodoDto }[];
    first?: Date;
    last?: Date;
}

export interface PaginatingTodosDto {
    items: TodoDto[];
    maxResultCount: number;
    skipCount: number;
    totalCount: number;
}