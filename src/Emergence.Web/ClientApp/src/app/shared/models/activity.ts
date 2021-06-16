import { ActivityType } from "./enums";
import { Photo } from "./photo";
import { Specimen } from "./specimen";
import { User } from "./user";

export class Activity {
  activityId: number;
  name: string;
  description: string;
  activityType: ActivityType;
  customActivityType: string;
  quantity: number;
  assignedTo: string;
  dateOccurred: Date;
  dateScheduled: Date;
  visibility: number;
  userId: number;
  createdBy: string;
  dateCreated: Date;
  modifiedBy: string;
  dateModified: Date;
  user: User;
  specimen: Specimen;
  specimenPhotos: Photo[];
  photos: Photo[];
  displayDate: Date;
}
