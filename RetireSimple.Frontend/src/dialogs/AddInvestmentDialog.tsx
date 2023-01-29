import {Dialog, DialogTitle} from '@mui/material';
import {FieldValues} from 'react-hook-form';
import {InvestmentDataForm} from '../forms/InvestmentDataForm';

export interface AddInvestmentDialogProps {
	open: boolean;
	onClose: () => void;
}

export const AddInvestmentDialog = (props: AddInvestmentDialogProps) => {

	const handleClose = () => props.onClose;

	const handleSubmit = (data: FieldValues) => {
		console.log(data);
		handleClose();
	};

	return (
		<Dialog onCanPlay={handleClose} open={props.open}>
			<DialogTitle>Add Investment</DialogTitle>
			<InvestmentDataForm onSubmit={handleSubmit} />
		</Dialog>
	);
};
