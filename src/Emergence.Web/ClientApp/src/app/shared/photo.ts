export interface Photo {
  photoId: number;
  type: number;
  typeId: number;
  filename: string;
  blobPath: string;
  contentType: string;
  width: number;
  height: number;
  dateTaken: Date;
  createdBy: string;
  dateCreated: Date;
  blobPathRoot: string;
  urlBroken: false;
  originalUri: string;
  largeUri: string;
  mediumUri: string;
  thumbnailUri: string;
}
