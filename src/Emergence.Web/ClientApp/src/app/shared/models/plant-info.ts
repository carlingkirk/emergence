import { ConservationStatus, Effect, LightType, LocationStatus, Month, SoilType, StratificationType, Unit, Visibility, WaterType, Wildlife } from "./enums";
import { Lifeform } from "./lifeform";
import { GeoLocation } from "./location";
import { Origin } from "./origin";
import { Photo } from "./photo";
import { Taxon } from "./taxon";
import { User } from "./user";

export class PlantInfo {
    plantInfoId: number;
    lifeformId: number;
    scientificName: string;
    commonName: string;
    bloomTime: BloomTime;
    height: Height;
    spread: Spread;
    requirements : Requirements;
    visibility: Visibility;
    name: string;
    notes: string;
    wildlifeEffects: WildlifeEffect[];
    userId: number;
    createdBy: string;
    dateCreated: Date;
    modifiedBy: string;
    dateModified: Date;
    origin: Origin;
    lifeform: Lifeform;
    taxon: Taxon;
    photos: Photo[];
    user: User;
    soilTypes: SoilType[];
    locations: PlantLocation[];
}
  
export class BloomTime {
  minimumBloomTime: Month;
  maximumBloomTime: Month;
}

export class Height {
  minimumHeight: number;
  maximumHeight: number;
  unit: Unit;
}

export class Spread {
  minimumSpread: number;
  maximumSpread: number;
  unit: Unit;
}

export class Requirements {
  waterRequirements: WaterRequirements;
  lightRequirements: LightRequirements;
  zoneRequirements: ZoneRequirements;
  stratificationStages: StratificationStage[];
}

export class WaterRequirements {
  minimumWater: WaterType;
  maximumWater: WaterType;
}

export class LightRequirements {
  minimumLight: LightType;
  maximumLight: LightType;
}

export class ZoneRequirements {
  minimumZone: Zone;
  maximumZone: Zone;
}

export class Zone {
  id: number;
  name: string;
}

export class StratificationStage {
  step: number;
  dayLength: number;
  stratificationType: StratificationType;
}

export class WildlifeEffect {
  wildlife: Wildlife;
  effect: Effect;
}

export class PlantLocation {
  location: GeoLocation;
  status: LocationStatus;
  conservationStatus: ConservationStatus;
  createdBy: string;
  dateCreated: Date;
  modifiedBy: string;
  dateModified: Date;
}