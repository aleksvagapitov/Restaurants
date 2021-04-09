import React from 'react'
import { observer } from 'mobx-react-lite'
import { Header, Grid, Image, Segment } from 'semantic-ui-react';
import { IRestaurant } from '../../../app/models/restaurant';

interface IProps {
    restaurant: IRestaurant
}

export const RestaurantDetailedPhotos: React.FC<IProps> = ({restaurant}) => {
    return (
        <Segment basic>
            {restaurant.photos.length > 0 && <Header>Photos</Header>}
            <Grid columns={4}>
                {restaurant.photos.slice(0, 4).map((photo) => (
                    <Grid.Column>
                        <Image src={photo.url}/>
                    </Grid.Column>
                ))}
            </Grid>
        </Segment>
    )
}

export default observer(RestaurantDetailedPhotos);
