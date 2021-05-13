import { Photo } from "./models/photo";

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