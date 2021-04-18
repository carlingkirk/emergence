import { InventoryItem } from "./inventory-item";

export interface Specimen {
  name: string;
  quantity: number;
  specimenId: number;
  specimenStage: string;
  createdBy: string;
  dateCreated: Date;
  modifiedBy: string;
  dateModified: Date;
  inventoryItem: InventoryItem;
}
