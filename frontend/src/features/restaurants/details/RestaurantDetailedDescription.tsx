import React, { useState } from 'react'
import { Header, Divider, Rating, Container, Segment } from 'semantic-ui-react'
import { IRestaurant } from '../../../app/models/restaurant'
import { observer } from 'mobx-react-lite'
import _ from 'lodash'

interface IProps {
    restaurant: IRestaurant
}

export const RestaurantDetailedDescription: React.FC<IProps> = ({restaurant}) => {
    const [starsAverage] = useState(Math.round(_.sumBy(restaurant.reviews, function (review){
        return review.stars
      })/restaurant.reviews.length))
      
    return (
        <Segment basic>
            <Header as="h1">{restaurant.name}</Header>
            <Divider/>
            <Rating size='large' defaultRating={starsAverage} disabled maxRating={5} />
            <span> {restaurant.reviews.length} Reviews</span>
            <Container text>
              {restaurant.description}
            </Container>
        </Segment>
    )
}

export default observer(RestaurantDetailedDescription);
