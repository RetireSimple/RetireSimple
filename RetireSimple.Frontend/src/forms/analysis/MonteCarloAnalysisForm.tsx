import {Grid, TextField, Typography} from '@mui/material';
import React from 'react';
import {Controller, useFormContext} from 'react-hook-form';

export const MonteCarloAnalysisForm = () => {
	const formContext = useFormContext();

	const {errors} = formContext.formState;

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

	//==============================================
	//Field definitions (To reduce indent depth)
	//==============================================
	const analysisLengthField = (
		<Controller
			name="analysisLength"
			control={formContext.control}
			render={({field}) => (
				<TextField {...field}
					label='Analysis Length (Months)'
					fullWidth
					size='small'
					error={!!errors.analysisLength}
					helperText={errors.analysisLength?.message as string}
				/>
			)} />);

	const simCountField = (
		<Controller
			name="simCount"
			control={formContext.control}
			render={({field}) => (
				<TextField {...field}
					label='Simulation Count'
					fullWidth
					size='small'
					error={!!errors.simCount}
					helperText={errors.simCount?.message as string}
				/>
			)} />);

	const randomVariableMuField = (
		<Controller
			name="randomVariableMu"
			control={formContext.control}
			render={({field}) => (
				<TextField {...field}
					label='Mu'
					fullWidth
					error={!!errors.randomVariableMu}
					size='small'
					helperText={errors.randomVariableMu?.message as string}
				/>
			)} />);

	const randomVariableSigmaField = (
		<Controller
			name="randomVariableSigma"
			control={formContext.control}
			render={({field}) => (
				<TextField {...field}
					label='Sigma'
					fullWidth
					error={!!errors.randomVariableSigma}
					size='small'
					helperText={errors.randomVariableSigma?.message as string}
				/>
			)} />);

	const randomVariableScaleFactorField = (
		<Controller
			name="randomVariableScaleFactor"
			control={formContext.control}
			render={({field}) => (
				<TextField {...field}
					label='Scale Factor'
					fullWidth
					error={!!errors.randomVariableScaleFactor}
					size='small'
					helperText={errors.randomVariableScaleFactor?.message as string}
				/>
			)} />);

	return (
		<Grid container spacing={2}>
			<Grid item xs={12}>
				<Typography variant='subtitle2' >Monte Carlo Analysis Parameters</Typography>
			</Grid>
			<Grid item xs={4}>{analysisLengthField}</Grid>
			<Grid item xs={4}>{simCountField}</Grid>
			<Grid item xs={12}>
				<Typography variant='subtitle2' >Random Variable Parameters (Normal)</Typography>
			</Grid>
			<Grid item xs={2}>{randomVariableMuField}</Grid>
			<Grid item xs={2}>{randomVariableSigmaField}</Grid>
			<Grid item xs={2}>{randomVariableScaleFactorField}</Grid>
		</Grid>
	);
};
