import { InventoryItem } from "./inventory-item";
import { Photo } from "./photo";

export interface Specimen {
  name: string;
  quantity: number;
  specimenId: number;
  specimenStage: string;
  notes: string;
  createdBy: string;
  dateCreated: Date;
  modifiedBy: string;
  dateModified: Date;
  inventoryItem: InventoryItem;
  photos: Photo[];
}
