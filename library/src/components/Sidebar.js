import React from 'react';
import { List, ListItem, ListItemText } from '@mui/material';

const Sidebar = () => {
  const items = ['Dashboard', 'Books', 'Library', 'Teacher', 'Students', 'Authors', 'Settings', 'Sign Out'];

  return (
    <List style={{height:"100vh"}}>
      {items.map((item, index) => (
        <ListItem  button  key={index}>
          <ListItemText primary={item} />
        </ListItem>
      ))}
    </List>
  );
};

export default Sidebar;
