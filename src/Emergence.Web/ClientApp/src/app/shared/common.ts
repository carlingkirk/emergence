import { Photo } from "./models/photo";
import { Specimen } from "./models/specimen";

export function onImgError(event, photo: Photo) {
    if (event.target.srcset) {
        event.src = photo.originalUri;
        event.target.src = photo.originalUri;
        event.target.srcset = '';
    } else {
        event.target.onerror = null;
        event.target.src = '/broken.png';
        event.target.srcset = '';
        event.stopPropagation();
    }
}

export function getElementId(element: string, id: string) {
    return element + "-" + id;
}

export function getSpecimenName(specimen: Specimen) {
    return specimen.lifeform?.commonName ?? specimen.name;
}

export function getSpecimenScientificName(specimen: Specimen) {
    return specimen.lifeform?.scientificName ?? "New";
}