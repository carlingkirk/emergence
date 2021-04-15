import { Origin } from "./origin";

export interface InventoryItem {
  inventoryItemId: number;
  itemType: number;
  name: string;
  quantity: number;
  status: number;
  dateAcquired: Date;
  visibility: number;
  userId: number;
  createdBy: string;
  dateCreated: Date;
  modifiedBy: string;
  dateModified: Date;
  origin: Origin;
}
