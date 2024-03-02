/**
 * Import function triggers from their respective submodules:
 *
 * import {onCall} from "firebase-functions/v2/https";
 * import {onDocumentWritten} from "firebase-functions/v2/firestore";
 *
 * See a full list of supported triggers at https://firebase.google.com/docs/functions
 */

import { onRequest } from "firebase-functions/v2/https";
import { getFirestore } from "firebase-admin/firestore";
import * as logger from "firebase-functions/logger";
import { initializeApp } from "firebase-admin/app";
import * as functions from "firebase-functions";

// Start writing functions
// https://firebase.google.com/docs/functions/typescript

initializeApp();

exports.logRun = onRequest(
  async (req: functions.https.Request, res: functions.Response<any>) => {
    const runData = req.body;

    try {
      const writeResult = await getFirestore() 
        .collection("messages")
        .add(runData);
      const message = `Message with ID: ${writeResult.id} was added.`;
      logger.log(message);
      res.json({ result: message });
    } catch (error) {
      console.error("Error writing document: ", error);
      res.status(500).send("Error writing document");
    }
  },
);
