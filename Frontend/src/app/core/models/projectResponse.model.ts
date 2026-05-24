export interface ProjectResponse {
    id: number;
    workspaceName: string;
    workspaceId: number;
    createdByName: string;
    createdById: number;
    name: string;
    description: string;
    status: string;
    createdAt: Date;
    updatedAt?: Date;
}