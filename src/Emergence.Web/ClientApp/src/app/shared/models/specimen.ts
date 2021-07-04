import { InventoryItemStatus, ItemType, SpecimenStage, Visibility } from './enums';
import { Inventory } from './inventory';
import { InventoryItem } from './inventory-item';
import { Lifeform } from './lifeform';
import { Photo } from './photo';

export class Specimen {
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

export function newSpecimen(userId: string, lifeform: Lifeform): Specimen {
  let specimen = new Specimen();
  let inventoryItemStatuses = Object.keys(InventoryItemStatus).filter(key => !isNaN(Number(key))).map(key => InventoryItemStatus[key]);
  let itemTypes = Object.keys(ItemType).filter(key => !isNaN(Number(key))).map((key) => ItemType[key]);
  let visibilities = Object.keys(Visibility).filter(key => !isNaN(Number(key))).map(key => Visibility[key]);

  specimen.specimenId = 0;
  specimen.lifeform = lifeform;
  specimen.createdBy = userId;
  specimen.ownerId = userId;
  specimen.dateCreated = new Date();
  specimen.inventoryItem = new InventoryItem();
  specimen.inventoryItem.inventory = new Inventory();
  specimen.inventoryItem.inventory.createdBy = userId;
  specimen.inventoryItem.inventory.dateCreated = new Date();
  specimen.photos = [];
  specimen.inventoryItem.status = inventoryItemStatuses[InventoryItemStatus['In Use']];
  specimen.inventoryItem.createdBy = userId;
  specimen.inventoryItem.dateCreated = new Date();
  specimen.inventoryItem.itemType = itemTypes[ItemType.Specimen];
  specimen.inventoryItem.visibility = visibilities[Visibility['Inherit from profile']];

  return specimen;
}
