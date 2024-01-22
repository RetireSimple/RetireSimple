import {Box, Typography} from '@mui/material';
import React from 'react';
import EditableTable from '../components/EditableTable';
import SettingsForm from '../components/SettingsComponent';
  
export function SettingsPage() { 
	return (<div>
		<h1>Account Settings</h1>
		<Box sx={{padding: '2rem'}}>
			<SettingsForm />
		</Box>
	</div>
	);
    
} 