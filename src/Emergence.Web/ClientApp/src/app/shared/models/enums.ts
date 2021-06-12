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

export enum Unit {
    "",
    "ft",
    "in",
    "m",
    "cm"
}

export enum WaterType {
    "",
    Dry,
    "Medium Dry",
    Medium,
    "Medium Wet",
    Wet
}

export enum LightType {
    "",
    "Full Shade",
    "Part Shade",
    "Part Sun",
    "Full Sun"
}

export enum SoilType {
    Fertile,
    Loamy,
    Rocky,
    Clay,
    Peaty,
    Swamp,
    Water
}

export enum StratificationType {
    "Refrigerate seed for storage",
    "Sand scarification",
    "Nick scarification",
    "Hot water treatment",
    "Cold moist stratification",
    "Warm moist stratification",
    "Needs light to germinate",
    "Sow in late fall",
    "Sow outdoors in fall",
    "Requires inoculum",
    "Requires host plant",
    "Fern spores"
}

export enum Month {
    "",
    Jan,
    Feb,
    Mar,
    Apr,
    May,
    Jun,
    Jul,
    Aug,
    Sep,
    Oct,
    Nov,
    Dec
}

export enum ConservationStatus {
    "" = 0,
    "Critically Endangered" = 1,
    Endangered = 2,
    Vulnerable = 3,
    "Near Threatened" = 4,
    "Least Concern" = 5,
    "Possibly Extirpated" = 6,
    "Presumed Extirpated" = 7,
    "Not Evaluated" = 97,
    "Not Applicable" = 98,
    "Data Deficient" = 99
}

export enum LocationStatus {
    "",
    Native,
    Introduced,
    Incidental,
    "Native & Introduced",
    "Native & Extirpated"
}

export enum Wildlife {
    Bees,
    Beetles,
    Birds,
    Butterflies,
    Hummingbirds,
    Moths,
    Wasps
  }
  
  export enum Effect {
    Food,
    Host
  }

export enum ActivityType {
    "",
    "Germinate",
    "Stratify",
    "Divide",
    "Take cutting",
    "Collect seeds",
    "Progress check",
    "Plant in ground",
    "Repot",
    "Water",
    "Fertilize",
    Custom,
    "Add to wishlist",
    Purchase
}

export enum OriginType {
    "",
    Nursery,
    Store,
    Location,
    Person,
    Event,
    Website,
    Webpage,
    Publication,
    File,
    Database
}

export enum Region {
    Africa,
    "Antarctica/Southern Ocean",
    Australia,
    Caribbean,
    "Europe & Northern Asia (excluding China)",
    "Middle America",
    "North America",
    "Oceania",
    "South America",
    "Southern Asia",
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