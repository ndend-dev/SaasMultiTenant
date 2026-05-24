import { User } from "./user.model";
import { Workspace } from "./workspace.model";

export interface LoginResponse {
    user: User;
    workspaces: Workspace[];
}