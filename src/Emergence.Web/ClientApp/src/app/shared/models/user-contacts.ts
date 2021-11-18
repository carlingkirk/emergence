import { UserSummary } from "./user";

export class UserContactRequest {
    id: number;
    userId: number;
    contactUserId: number;
    dateRequested: Date;
    user: UserSummary;
    contactUser: UserSummary;
}

export class UserContact {
    id: number;
    userId: number;
    contactUserId: number;
    dateRequested: Date;
    dateAccepted: Date;
    user: UserSummary;
    contactUser: UserSummary;
}