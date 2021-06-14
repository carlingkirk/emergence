import { GeoLocation } from "./location";
import { User } from "./user";

export class Origin {
  originId: number;
  name: string;
  type: number;
  description: string;
  authors: string;
  externalId: string;
  altExternalId: string;
  uri: string;
  visibility: number;
  userId: number;
  createdBy: string;
  dateCreated: Date;
  modifiedBy: string;
  dateModified: Date;
  shortUri: string;
  tinyUri: string;
  location: GeoLocation;
  parentOrigin: Origin;
  user: User;
}
