export interface Notification {
    id: string;

    userId: string;
    alertId: string;

    channel: number;
    channelCode: number;
    channelName: string;

    status: number;
    statusCode: number;
    statusName: string;

    subject: string;
    message: string;

    isRead: boolean;

    createdAtUtc: string;
    sentAtUtc?: string;

    errorMessage?: string;
}