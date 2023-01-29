import {Grid, TextField, Typography} from '@mui/material';
import React from 'react';
import {Controller, useFormContext} from 'react-hook-form';

export const MonteCarloAnalysisForm = () => {
	const formContext = useFormContext();

	React.useEffect(() => {
		return () => {
			formContext.unregister('analysisLength');
			formContext.unregister('simCount');
			formContext.unregister('randomVariableMu');
			formContext.unregister('randomVariableSigma');
			formContext.unregister('randomVariableScaleFactor');
		};
		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, []);

	return (
		<Grid container spacing={2}>
			<Grid item xs={12}>
				<Typography variant='subtitle2' >Monte Carlo Analysis Parameters</Typography>
			</Grid>
			<Grid item xs={4}>
				<Controller
					name="analysisLength"
					control={formContext.control}
					render={({field}) => (
						<TextField {...field} label='Analysis Length (Months)' fullWidth />
					)} />
			</Grid>
			<Grid item xs={4}>
				<Controller
					name="simCount"
					control={formContext.control}
					render={({field}) => (
						<TextField {...field} label='Simulation Count' fullWidth />
					)} />
			</Grid>
			<Grid item xs={12}>
				<Typography variant='subtitle2' >Random Variable Parameters (Normal)</Typography>
			</Grid>
			<Grid item xs={4}>
				<Controller
					name="randomVariableMu"
					control={formContext.control}
					render={({field}) => (
						<TextField {...field} label='Mu' fullWidth />
					)} />
			</Grid>
			<Grid item xs={4}>
				<Controller
					name="randomVariableSigma"
					control={formContext.control}
					render={({field}) => (
						<TextField {...field} label='Sigma' fullWidth />
					)} />
			</Grid>
			<Grid item xs={4}>
				<Controller
					name="randomVariableScaleFactor"
					control={formContext.control}
					render={({field}) => (
						<TextField {...field} label='Scale Factor' fullWidth />
					)} />
			</Grid>
		</Grid>
	);
};
