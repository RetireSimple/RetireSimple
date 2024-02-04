import {Box, Typography} from '@mui/material';
import React from 'react';
import EditableTable from '../components/EditableTable';
import SettingsForm from '../components/SettingsComponent';
import SettingsNav from './SettingsNav';
  
export function SettingsPage() { 
	return (<div>
		<SettingsNav />
		<h1>Account Settings</h1>
		<Box sx={{padding: '2rem'}}>
			<SettingsForm />
		</Box>
	</div>
	);
    
} 