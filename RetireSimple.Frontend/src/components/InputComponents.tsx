import {
	Box,
	FormControl,
	FormHelperText,
	InputAdornment,
	InputLabel,
	MenuItem,
	Select,
	TextField,
	Tooltip,
} from '@mui/material';
import {DatePicker, LocalizationProvider} from '@mui/x-date-pickers';
import {AdapterDayjs} from '@mui/x-date-pickers/AdapterDayjs';
import dayjs from 'dayjs';
import React from 'react';
import {Control, Controller} from 'react-hook-form';

export interface FormTextFieldProps {
	name: string;
	label: string;
	control: Control;
	errorField: any;
	defaultValue: string;
	tooltip?: string | React.ReactNode;
	// decoration?: 'currency' | 'percent';
}

export interface FormSelectFieldProps {
	name: string;
	label: string;
	options: {value: string; label: string; tooltip: string}[];
	defaultOption: string; //Assert it is in options for easy programming
	control: Control;
	errorField: any;
	disable: boolean;
	tooltip?: string | React.ReactNode;
}

export interface FormDatePickerProps {
	name: string;
	label: string;
	control: Control;
	errorField: any;
	defaultValue: string;
	tooltip?: string | React.ReactNode;
}

export const FormTextField = (props: FormTextFieldProps) => {
	return (
		<Controller
			name={props.name}
			control={props.control}
			defaultValue={props.defaultValue}
			render={({field}) => (
				<Tooltip title={props.tooltip ?? ''} arrow describeChild placement='top'>
					<TextField
						{...field}
						label={props.label}
						fullWidth
						size='small'
						error={!!props.errorField}
						helperText={props.errorField?.message as string}
					/>
				</Tooltip>
			)}
		/>
	);
};

export const FormTextFieldCurrency = (props: FormTextFieldProps) => {
	return (
		<Controller
			name={props.name}
			control={props.control}
			defaultValue={props.defaultValue}
			render={({field}) => (
				<Tooltip title={props.tooltip ?? ''} arrow describeChild placement='top'>
					<TextField
						{...field}
						label={props.label}
						fullWidth
						size='small'
						error={!!props.errorField}
						helperText={props.errorField?.message as string}
						InputProps={{
							startAdornment: <InputAdornment position='start'>$</InputAdornment>,
						}}
					/>
				</Tooltip>
			)}
		/>
	);
};

export const FormTextFieldPercent = (props: FormTextFieldProps) => {
	return (
		<Controller
			name={props.name}
			control={props.control}
			defaultValue={props.defaultValue}
			render={({field}) => (
				<Tooltip title={props.tooltip ?? ''} arrow describeChild placement='top'>
					<TextField
						{...field}
						label={props.label}
						fullWidth
						size='small'
						error={!!props.errorField}
						helperText={props.errorField?.message as string}
						InputProps={{
							endAdornment: <InputAdornment position='end'>%</InputAdornment>,
						}}
					/>
				</Tooltip>
			)}
		/>
	);
};

export const FormTextFieldMonthUnits = (props: FormTextFieldProps) => {
	return (
		<Controller
			name={props.name}
			control={props.control}
			defaultValue={props.defaultValue}
			render={({field}) => (
				<Tooltip title={props.tooltip ?? ''} arrow describeChild placement='top'>
					<TextField
						{...field}
						label={props.label}
						fullWidth
						size='small'
						error={!!props.errorField}
						helperText={props.errorField?.message as string}
						InputProps={{
							endAdornment: <InputAdornment position='end'>mo.</InputAdornment>,
						}}
					/>
				</Tooltip>
			)}
		/>
	);
};

export const FormSelectField = (props: FormSelectFieldProps) => {
	return (
		<Controller
			name={props.name}
			control={props.control}
			defaultValue={props.defaultOption}
			render={({field}) => (
				<Tooltip title={props.tooltip ?? ''} arrow describeChild placement='top'>
					<FormControl fullWidth error={!!props.errorField} size='small'>
						<InputLabel id={props.name}>{props.label}</InputLabel>
						<Select {...field} label={props.name} disabled={props.disable}>
							{props.options.map((option) => (
								// <Tooltip title={option.tooltip}>
								<MenuItem key={option.value} value={option.value}>
									{/* {option.label} */}
									{<Tooltip title={option.tooltip}>
										{<div>{option.label}</div>}
									</Tooltip>}
								</MenuItem>
								// </Tooltip>
							))}
						</Select>
						<FormHelperText>{props.errorField?.message as string}</FormHelperText>
					</FormControl>
				</Tooltip>
			)}
		/>
	);
};

export const FormDatePicker = (props: FormDatePickerProps) => {
	return (
		<Tooltip title={props.tooltip ?? ''} arrow describeChild placement='top'>
			<Box>
				<Controller
					name={props.name}
					control={props.control}
					defaultValue={props.defaultValue ?? ''}
					render={({field}) => (
						<LocalizationProvider dateAdapter={AdapterDayjs}>
							<DatePicker
								{...field}
								label={props.label}
								value={dayjs(field.value)}
								slotProps={{
									textField: {
										size: 'small',
										error: !!props.errorField,
										helperText: props.errorField?.message as string,
									},
								}}
							/>
						</LocalizationProvider>
					)}
				/>
			</Box>
		</Tooltip>
	);
};
