import {Grid, Typography} from '@mui/material';
import React from 'react';
import {useFormContext} from 'react-hook-form';
import {FormTextField} from '../Inputs';

export const MonteCarloAnalysisForm = () => {
	const formContext = useFormContext();
	const {errors} = formContext.formState;

	//==============================================
	//Field definitions (To reduce indent depth)
	//==============================================
	const analysisLengthField = (
		<FormTextField
			name='analysisLength'
			label='Analysis Length (Months)'
			control={formContext.control}
			errorField={errors.analysisLength}
		/>);

	const simCountField = (
		<FormTextField
			name='simCount'
			label='Simulation Count'
			control={formContext.control}
			errorField={errors.simCount}
		/>);

	const randomVariableMuField = (
		<FormTextField
			name='randomVariableMu'
			label='Mu'
			control={formContext.control}
			errorField={errors.randomVariableMu}
		/>);

	const randomVariableSigmaField = (
		<FormTextField
			name='randomVariableSigma'
			label='Sigma'
			control={formContext.control}
			errorField={errors.randomVariableSigma}
		/>);

	const randomVariableScaleFactorField = (
		<FormTextField
			name='randomVariableScaleFactor'
			label='Scale Factor'
			control={formContext.control}
			errorField={errors.randomVariableScaleFactor}
		/>);


	return (
		<Grid container spacing={2}>
			<Grid item xs={12}>
				<Typography variant='subtitle2'>Monte Carlo Analysis Parameters</Typography>
			</Grid>
			<Grid item xs={4}>{analysisLengthField}</Grid>
			<Grid item xs={4}>{simCountField}</Grid>
			<Grid item xs={12}>
				<Typography variant='subtitle2'>Random Variable Parameters (Normal)</Typography>
			</Grid>
			<Grid item xs={2}>{randomVariableMuField}</Grid>
			<Grid item xs={2}>{randomVariableSigmaField}</Grid>
			<Grid item xs={2}>{randomVariableScaleFactorField}</Grid>
		</Grid>
	);
};
