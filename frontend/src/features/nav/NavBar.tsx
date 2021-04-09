import React, { useContext, Fragment } from "react";
import { Menu, Button, Dropdown, Image, Divider } from "semantic-ui-react";
import { LoginForm } from "../user/LoginForm";
import { RootStoreContext } from "../../app/stores/rootStore";
import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import RegisterForm from "../user/RegisterForm";
import { CityDropdown } from "./CityDropdown";

const NavBar: React.FC = () => {
  const rootStore = useContext(RootStoreContext);
  const { user, isOwner, isUser, isAdmin, logout } = rootStore.userStore;
  const { openModal } = rootStore.modalStore;
  return (
    <Fragment>
      <Menu secondary style={{ height: "4px" }}>
        <Menu.Item header as={Link} to={`/`}>
          <img src="./assets/logo.png" alt="logo" style={{ marginRight: 10 }} />
          Restaurants
        </Menu.Item>
        <Menu.Item>
          <CityDropdown /> 
        </Menu.Item>
        <Menu.Item></Menu.Item>

        {user ? (
          <Menu.Item position="right" style={{ marginRight: "6em" }}>
            <Image
              avatar
              spaced="right"
              src={user.image || "./assets/user.png"}
            />
            <Dropdown pointing="top left" text={user.displayName}>
              <Dropdown.Menu>
                {isOwner && (
                  <Dropdown.Item
                    as={Link}
                    to={`/owner`}
                    text="Owner Panel"
                  />
                )}
                {isAdmin && (
                  <Dropdown.Item
                    as={Link}
                    to={`/admin`}
                    text="Admin Panel"
                  />
                )}
                {isUser && (
                  <Fragment>
                    <Dropdown.Item
                      as={Link}
                      to={`/profile/${user.username}`}
                      text="My profile"
                      icon="user"
                    />
                    <Dropdown.Item
                      as={Link}
                      to={"/account"}
                      text="My Account"
                    />
                  </Fragment>
                )}
                <Dropdown.Item onClick={logout} text="Logout" icon="power" />
              </Dropdown.Menu>
            </Dropdown>
          </Menu.Item>
        ) : (
          <Menu.Menu position="right" style={{ marginRight: "6em" }}>
            <Menu.Item>
              <Button primary onClick={() => openModal(<RegisterForm />)}>
                Sign up
              </Button>
            </Menu.Item>
            <Menu.Item onClick={() => openModal(<LoginForm />)}>
              Sign in
            </Menu.Item>
          </Menu.Menu>
        )}
      </Menu>
      <Divider />
    </Fragment>
  );
};

export default observer(NavBar);
