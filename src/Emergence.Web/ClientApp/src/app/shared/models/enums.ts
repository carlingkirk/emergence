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