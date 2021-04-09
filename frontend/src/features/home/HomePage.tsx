import React, { Fragment } from 'react'
import { observer } from 'mobx-react-lite'
import Showcase from './Showcase';
import SearchForm from '../restaurants/dashboard/SearchForm';
import { RouteComponentProps } from 'react-router-dom';

export const HomePage: React.FC<RouteComponentProps> = ({match, location, history}) => {
    return (
        <Fragment>
            <SearchForm match={match} history={history} location={location} />
            <Showcase  term="Japanese"/>
            <Showcase  term="Steak"/>
            <Showcase  term="Ramen"/>
        </Fragment>
    )
}

export default observer(HomePage);