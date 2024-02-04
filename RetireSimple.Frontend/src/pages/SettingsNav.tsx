
import { Link, useLocation } from 'react-router-dom';
import {Tab, Tabs} from '@mui/material';
import React, { useEffect, useState } from 'react';

const SettingsNav = () => {
	const [tab, setTab] = useState(0);
	const [value, setValue] = useState('');

	const handleChange = (event: React.SyntheticEvent, newValue: string) => {
		console.log(newValue);
		setValue(newValue);
		console.log(value);
	};

	return (
		<Tabs 
			value={value}
			onChange={handleChange}

			// onChange={(e, v) => {
			// 	console.log(v);
			// 	setTab(v);
			// 	//ChangeTab(v);
			// 	console.log(tab);
			// }}
		>
			<Tab value='Account Settings' label='Account Settings' component={Link} to='/Settings' />
			<Tab value='Engine Info' label='Engine Info' component={Link} to='/EngineInfoPage' />
			<Tab value='About' label='About' component={Link} to='/AboutPage' />
			<Tab value='Help' label='Help' component={Link} to='/HelpPage' />
		</Tabs>
	);
}
  

{/* <Tabs value={tab} onChange={(e, v) => setTab(v)}>
				<Tab label='Investment Details' />
				<Tab label='Expense Information' />
			</Tabs> */}
export default SettingsNav;