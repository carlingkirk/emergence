import { InventoryItem } from "./inventory-item";

export interface Inventory {
  inventoryId: number;
  ownerId: string;
  items: InventoryItem[];
  createdBy: string;
  dateCreated: Date;
}
