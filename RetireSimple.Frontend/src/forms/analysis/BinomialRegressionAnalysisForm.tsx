import React from 'react';
import {useFormContext, useWatch} from 'react-hook-form';
import {PresetContext} from '../../Layout';
import {addSpacesCapitalCase} from '../../api/ConvertUtils';
import {
	FormSelectField,
	FormTextFieldMonthUnits,
	FormTextFieldPercent,
} from '../../components/InputComponents';
import {FormControlLabel, Grid, Switch, Typography} from '@mui/material';

export const BinomialRegressionAnalysisForm = () => {
	const formContext = useFormContext();
	const {errors} = formContext.formState;
	const presets = React.useContext(PresetContext)?.['BinomialRegression'];
	const currentPreset = useWatch({
		control: formContext.control,
		name: 'analysis_analysisPreset',
		defaultValue: formContext.getValues('analysis_analysisPreset'),
	});
	const [showSettings, setShowSettings] = React.useState(false);
	const [showAdvanced, setShowAdvanced] = React.useState(false);

	React.useEffect(() => {
		setShowAdvanced(currentPreset === 'Custom');
	}, [currentPreset]);

	const presetOptions = React.useMemo(() => {
		if (!presets)
			return [{label: 'Custom', value: 'Custom'}] as {label: string; value: string}[];
		const presetList: {label: string; value: string}[] = Object.keys(presets).map(
			(presetName) => ({
				label: addSpacesCapitalCase(presetName),
				value: presetName,
			}),
		);

		presetList.push({label: 'Custom', value: 'Custom'});
		return presetList;
	}, [presets]);

	//==============================================
	//Field definitions (To reduce indent depth)
	//==============================================

	const analysisLengthField = (
		<FormTextFieldMonthUnits
			name='analysis_analysisLength'
			label='Analysis Length'
			control={formContext.control}
			errorField={errors.analysisLength}
			tooltip='The number of months from today to run the analysis for.'
		/>
	);

	const analysisPresetField = (
		<FormSelectField
			name='analysis_analysisPreset'
			label='Analysis Preset'
			control={formContext.control}
			errorField={errors.analysis_analysisPreset}
			tooltip='A preset to use for the analysis.'
			options={presetOptions}
			defaultOption={''}
			disable={false}
		/>
	);

	const percentGrowthField = (
		<FormTextFieldPercent
			name='analysis_percentGrowth'
			label='Percent Growth'
			control={formContext.control}
			errorField={errors.analysis_percentGrowth}
			tooltip='The percent growth to use for the analysis.'
		/>
	);

	const uncertaintyField = (
		<FormTextFieldPercent
			name='analysis_uncertainty'
			label='Uncertainty'
			control={formContext.control}
			errorField={errors.analysis_uncertainty}
			tooltip='The uncertainty to use for the analysis.'
		/>
	);

	const getPresetValues = (field: string) => {
		if (!presets || !currentPreset || !Object.keys(presets).includes(currentPreset))
			return 'Not Set';
		return presets[currentPreset][field];
	};

	return (
		<>
			<Grid item xs={4}>
				{analysisLengthField}
			</Grid>
			<Grid item xs={4}>
				{analysisPresetField}
			</Grid>
			{!showAdvanced && (
				<>
					<Grid item xs={4}>
						<FormControlLabel
							control={
								<Switch
									checked={showSettings}
									onChange={() => setShowSettings(!showSettings)}
								/>
							}
							label='Show Settings'
						/>
					</Grid>

					{showSettings && (
						<Grid item xs={12}>
							<Typography variant='subtitle2'>
								Summary of This Analysis Method
							</Typography>
							<Typography variant='body2'>{`
								This method is a simple binomial tree regression analysis for Put Options.
								It is based on the idea that the next month's price is either a fixed percent
								higher or lower than the current price. This creates a "tree" that branches
								in a binomial fashion (in a very similar pattern to Pascal's Triangle) and
								each node represents a possible price. This grows exponentially, so this version of
								algorithm trims the "center of the tree" as the analysis currently assumes
								 a symmetrical increase/decrease percentage.
							`}</Typography>
							<br />
							<Typography variant='body2'>
								This method is generally faster to compute than a Monte Carlo
								Simulation, but it is less accurate.
							</Typography>
							<Typography variant='h6'>Binomial Regression Parameters</Typography>
							<Typography variant='body2'>
								{`Expected Percent Growth: ${getPresetValues(
									'analysis_percentGrowth',
								)}`}
							</Typography>
							<Typography variant='body2'>
								{`Uncertainty: ${getPresetValues('analysis_uncertainty')}`}
							</Typography>
						</Grid>
					)}
				</>
			)}
			{showAdvanced && (
				<>
					<Grid item xs={12}>
						<Typography variant='h6'>Binomial Regression Parameters</Typography>
					</Grid>
					<Grid item xs={4}>
						{percentGrowthField}
					</Grid>

					<Grid item xs={4}>
						{uncertaintyField}
					</Grid>
				</>
			)}
		</>
	);
};
