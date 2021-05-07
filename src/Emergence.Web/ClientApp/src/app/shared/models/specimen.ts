import { InventoryItem } from "./inventory-item";
import { Lifeform } from "./lifeform";
import { Photo } from "./photo";

export interface Specimen {
  name: string;
  quantity: number;
  specimenId: number;
  specimenStage: string;
  notes: string;
  ownerId: string;
  createdBy: string;
  dateCreated: Date;
  modifiedBy: string;
  dateModified: Date;
  inventoryItem: InventoryItem;
  lifeform: Lifeform;
  photos: Photo[];
}
