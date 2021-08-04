import { UserSummary } from "./user";

export class UserContactRequest {
    id: string;
    userId: string;
    contactUserId: string;
    dateRequested: string;
    user: UserSummary;
    contactUser: UserSummary;
}

export class UserContact {
    id: string;
    userId: string;
    contactUserId: string;
    dateRequested: string;
    dateAccepted: string;
    user: UserSummary;
    contactUser: UserSummary;
}