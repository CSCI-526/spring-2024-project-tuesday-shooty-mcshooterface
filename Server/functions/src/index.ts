import { onRequest } from "firebase-functions/v2/https";
import { getFirestore } from "firebase-admin/firestore";
import * as logger from "firebase-functions/logger";
import { initializeApp } from "firebase-admin/app";
import * as functions from "firebase-functions";
import * as admin from "firebase-admin";

initializeApp();
const RUN_COLLECTION_NAME = "runs";

// This returns all the runs in the database. We can query this on the client side to visualize data.
exports.allRuns = onRequest(
  { cors: true },
  async (_: functions.https.Request, res: functions.Response<any>) => {
    try {
      const collectionRef = admin.firestore().collection(RUN_COLLECTION_NAME);
      const querySnapshot = await collectionRef.get();
      const allDocuments: any[] = [];
      querySnapshot.forEach((doc) => {
        allDocuments.push(doc.data());
      });

      res.status(200).json({ data: allDocuments });
    } catch (error) {
      console.error("Error getting documents: ", error);
      res.status(500).send("Error getting documents");
    }
  },
);

// This logs a single game run. We can store all the analytics data on firestore from here.
exports.logRun = onRequest(
  { cors: true },
  async (req: functions.https.Request, res: functions.Response<any>) => {
    const runData = req.body;

    try {
      const writeResult = await getFirestore()
        .collection(RUN_COLLECTION_NAME)
        .add(runData);
      const message = `Run with ID: ${writeResult.id} was added.`;
      logger.log(message);
      res.json({ result: message });
    } catch (error) {
      console.error("Error writing document: ", error);
      res.status(500).send("Error writing document");
    }
  },
);

exports.downloadDatabase = onRequest(
  { cors: true },
  async (_: functions.https.Request, res: functions.Response<any>) => {
    try {
      const collectionRef = admin.firestore().collection(RUN_COLLECTION_NAME);
      const querySnapshot = await collectionRef.get();
      const allDocuments: any[] = [];
      for (let i = 0; i < querySnapshot.docs.length; i++) {
        const data = querySnapshot.docs[i].data();
        const id = querySnapshot.docs[i].id;
        allDocuments.push({
          id: id,
          data: data,
        });
      }

      res.status(200).json({ data: allDocuments });
    } catch (error) {
      console.error("Error getting documents: ", error);
      res.status(500).send("Error getting documents");
    }
  },
);
