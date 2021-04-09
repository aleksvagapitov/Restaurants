import React, { useContext, useState } from "react";
import { Tab, Header, Card, Image, Button, Grid } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { RootStoreContext } from "../../../app/stores/rootStore";
import PhotoUploadWidget from "../../../app/common/photoUpload/PhotoUploadWidget";

const OwnerRestaurantPhotos = () => {
  const rootStore = useContext(RootStoreContext);
  const {
    restaurant,
    uploadPhoto,
    uploadingPhoto,
    setMainPhoto,
    deletePhoto,
    loading
  } = rootStore.ownerRestaurant;

  const [addPhotoMode, setAddPhotoMode] = useState(false);
  const [target, setTarget] = useState<string | undefined>(undefined);
  const [deleteTarget, setDeleteTarget] = useState<string | undefined>(
    undefined
  );

  const handleUploadImage = (photo: Blob) => {
    if (restaurant)
      uploadPhoto(restaurant.id, photo).then(() => setAddPhotoMode(false));
  };

  return (
    <Tab.Pane>
      <Grid>
        <Grid.Column width={16} style={{ paddingBottom: 0 }}>
          <Header floated="left" icon="image" content="Photos" />
            <Button
              floated="right"
              basic
              content={addPhotoMode ? "Cancel" : "Add Photo"}
              onClick={() => setAddPhotoMode(!addPhotoMode)}
            />
        </Grid.Column>
        <Grid.Column width={16}>
          {addPhotoMode ? (
            <PhotoUploadWidget
              uploadPhoto={handleUploadImage}
              loading={uploadingPhoto}
            />
          ) : (
            <Card.Group itemsPerRow={5}>
              {restaurant &&
                restaurant.photos.map(photo => (
                  <Card key={photo.id}>
                    <Image src={photo.url} />
                      <Button.Group fluid widths={2}>
                        <Button
                          name={photo.id}
                          onClick={e => {
                            setMainPhoto(restaurant.id, photo);
                            setTarget(e.currentTarget.name);
                          }}
                          disabled={photo.isMain}
                          loading={loading && target === photo.id}
                          basic
                          positive
                          content="Main"
                        ></Button>
                        <Button
                          name={photo.id}
                          disabled={photo.isMain}
                          onClick={(e) => {
                            deletePhoto(restaurant.id, photo);
                            setDeleteTarget(e.currentTarget.name);
                          }}
                          loading={loading && deleteTarget === photo.id}
                          basic
                          negative
                          icon="trash"
                        ></Button>
                      </Button.Group>
                  </Card>
                ))}
            </Card.Group>
          )}
        </Grid.Column>
      </Grid>
    </Tab.Pane>
  );
};

export default observer(OwnerRestaurantPhotos);
