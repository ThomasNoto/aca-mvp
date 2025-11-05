import { UserRoles } from './user-role.enum';

export interface User {
    id : number;
    name: string;
    role: UserRoles;
}