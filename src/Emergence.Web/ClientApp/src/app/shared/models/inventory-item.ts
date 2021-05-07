import { User } from "./user";
import { Origin } from "./origin";
import { Inventory } from "./inventory";
import { ItemType } from "./enums";

export interface InventoryItem {
  inventoryItemId: number;
  itemType: ItemType;
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
  inventory: Inventory;
  origin: Origin;
  user: User;
}
