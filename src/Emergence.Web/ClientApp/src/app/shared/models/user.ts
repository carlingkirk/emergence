import { Visibility } from "./enums";
import { GeoLocation } from "./location";

export interface User {
  id: number;
  userId: string;
  displayName: string;
  photoId: number;
  photoThumbnailUri: string;
  location: GeoLocation;
  bio: string;
  profileVisibility: Visibility;
  inventoryItemVisibility: Visibility;
  plantInfoVisibility: Visibility;
  originVisibility: Visibility;
  activityVisibility: Visibility;
  isViewerContact: boolean;
  isViewerContactRequested: boolean;
}

export interface UserSummary {
  id: number;
  displayName: string;
  photoId: number;
  photoThumbnailUri: string;
  profileVisibility: Visibility;
  inventoryItemVisibility: Visibility;
  plantInfoVisibility: Visibility;
  originVisibility: Visibility;
  activityVisibility: Visibility;
}
