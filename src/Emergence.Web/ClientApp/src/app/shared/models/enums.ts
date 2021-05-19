export enum SpecimenStage {
    'Unknown',
    'Seed',
    'Ordered',
    'Stratification',
    'Germination',
    'Growing',
    'In Ground',
    'Blooming',
    'Diseased',
    'Deceased',
    'Dormant'
}

export enum InventoryItemStatus {
    'Available',
    'Wishlist',
    'Ordered',
    'In Use'
}

export enum Visibility {
    'Inherit from profile',
    'Public',
    'My Contacts',
    'Private'
}

export enum ItemType {
    Specimen,
    Supply,
    Container,
    Tool
}

export enum PhotoType {
    Activity,
    Specimen,
    'Inventory Item',
    Origin,
    'Plant Profile',
    User
}

export enum SortDirection {
    None,
    Ascending,
    Descending
}

export function getSortDirectionValues() {
    return Object.keys(SortDirection).filter(key => !isNaN(Number(key))).map(key => SortDirection[key]);
}

export function getSpecimenStageValues() {
    return Object.keys(SpecimenStage).filter(key => !isNaN(Number(key))).map(key => SpecimenStage[key]);
}

export function getInventoryItemStatusValues() {
    return Object.keys(InventoryItemStatus).filter(key => !isNaN(Number(key))).map(key => InventoryItemStatus[key]);
}

export function getVisibilityValues() {
    return Object.keys(Visibility).filter(key => !isNaN(Number(key))).map(key => Visibility[key]);
}

export function getItemTypeValues() {
    return Object.keys(ItemType).filter(key => !isNaN(Number(key))).map(key => ItemType[key]);
}

export function getPhotoTypeValues() {
    return Object.keys(PhotoType).filter(key => !isNaN(Number(key))).map(key => PhotoType[key]);
}