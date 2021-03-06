﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellWeaver : GestureHandler
{
    public ElementSelectionSpell CurrentElement { get; set; }

    [SerializeField]
    private ElementSelectionSpell m_DefaultElement;

    [SerializeField]
    private List<Gesture> m_RecognizableSpells = new List<Gesture>();

    [SerializeField]
    private Spellbook m_Spellbook;

    private void Awake()
    {
        m_DefaultElement.Cast(this);
    }

    protected override void handleGesture()
    {
        List<Tuple<double, Gesture>> similarGestures = new List<Tuple<double, Gesture>>();

        foreach (Gesture recognizableGesture in m_RecognizableSpells)
        {
            if (RecordedGesture.IsSimilarTo(recognizableGesture, Gesture.eAccuracyLevel.Position))
            {
                similarGestures.Add(new Tuple<double, Gesture>(RecordedGesture.SimilarityScoreAgainst(recognizableGesture, Gesture.eAccuracyLevel.Position), recognizableGesture));

                Debug.Log(recognizableGesture.name + " " + RecordedGesture.SimilarityScoreAgainst(recognizableGesture, Gesture.eAccuracyLevel.Position));
            }
        }
        Debug.Log("");


        Gesture bestMatch = similarGestures.OrderBy(gestureScore => gestureScore.Item1)
                                           .FirstOrDefault()?
                                           .Item2;

        if (bestMatch != null)
        {
            bestMatch.RepresentedSpell.Cast(this);

            if (bestMatch.RepresentedSpell is ElementSelectionSpell)
            {
                CurrentElement = bestMatch.RepresentedSpell as ElementSelectionSpell;
            }
            else if (bestMatch.RepresentedSpell is FormSelectionSpell)
            {
                CombineSpells(CurrentElement, bestMatch.RepresentedSpell as FormSelectionSpell);
            }
        }
    }

    public void CombineSpells(ElementSelectionSpell i_Element, FormSelectionSpell i_SpellToCombine)
    {
        m_Spellbook.GetSpell(i_Element, i_SpellToCombine)?.Cast(this);
    }
}
