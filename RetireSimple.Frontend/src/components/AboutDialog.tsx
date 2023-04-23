import {Box, Button, Dialog, DialogActions, DialogTitle, Link, Typography} from '@mui/material';

export interface AboutDialogProps {
	open: boolean;
	onClose: () => void;
}

export const AboutDialog = (props: AboutDialogProps) => {
	return (
		<Dialog open={props.open} maxWidth='md'>
			<DialogTitle>About RetireSimple</DialogTitle>
			<Box sx={{padding: '2rem'}}>
				<Typography variant='body1'>RetireSimple v1.3.0</Typography>
				<Typography variant='body1'>
					© 2022-2023 Alex Westerman, Ryan Nikolic, All Rights Reserved
				</Typography>
				<br />
				<Typography variant='body1'>
					This software and its source code is licensed under the{' '}
					<Link href='https://opensource.org/licenses/MIT'>MIT License</Link>.
				</Typography>
				<Typography variant='body1'>
					Source Code is available on{' '}
					<Link href='https://github.com/RetireSimple/RetireSimple'>GitHub</Link>.
				</Typography>
				<br />
				<Typography variant='h6'>DISCLAIMER</Typography>
				<Typography variant='body1'>
					<strong>
						The information provided by RetireSimple is for informational purposes only.
						It should not be considered as solid financial advice and should not be the
						only source of information you use to make financial decisions. You should
						consult with a certified financial professional (especially one that is a
						certified fiduciary) before making any financial decisions or taking any
						actions related to your finances.
					</strong>
				</Typography>
			</Box>
			<DialogActions>
				<Button onClick={props.onClose}>Close</Button>
			</DialogActions>
		</Dialog>
	);
};
