﻿using Server;
using System.Collections.Generic;


namespace Scripts.Mythik.Systems.Achievements.Items
{
    public class AcheivmentSystemMemoryStone : Item
    {
        public static AcheivmentSystemMemoryStone GetInstance()
        {
            if (m_instance == null)
                m_instance = new AcheivmentSystemMemoryStone();
            m_instance.MoveToWorld(new Point3D(0, 0, 0), Map.Felucca);
            return m_instance;
        }
        internal Dictionary<Serial, Dictionary<int, AcheiveData>> Achievements = new Dictionary<Serial, Dictionary<int, AcheiveData>>();
        internal Dictionary<Serial, int> PointsTotals = new Dictionary<Serial, int>();
        private static AcheivmentSystemMemoryStone m_instance;



        [Constructable]
        public AcheivmentSystemMemoryStone() : base(0xED4)
        {
            Visible = false;
            Name = "AcheivmentSystemStone DO NOT REMOVE";
            m_instance = this;
        }

        public AcheivmentSystemMemoryStone(Serial serial) : base(serial)
        {
            m_instance = this;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version 

            writer.Write(PointsTotals.Count);
            foreach (var kv in PointsTotals)
            {
                writer.Write(kv.Key);
                writer.Write(kv.Value);
            }

            writer.Write(Achievements.Count);
            foreach (var kv in Achievements)
            {
                writer.Write(kv.Key);
                writer.Write(kv.Value.Count);
                foreach (var ac in kv.Value)
                {
                    writer.Write(ac.Key);
                    ac.Value.Serialize(writer);
                }
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            int count = reader.ReadInt();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    PointsTotals.Add(reader.ReadInt(), reader.ReadInt());
                }
            }

            count = reader.ReadInt();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Serial id = reader.ReadInt();
                    var dict = new Dictionary<int, AcheiveData>();
                    int iCount = reader.ReadInt();
                    if (iCount > 0)
                    {
                        for (int x = 0; x < iCount; x++)
                        {
                            dict.Add(reader.ReadInt(), new AcheiveData(reader));
                        }

                    }
                    Achievements.Add(id, dict);
                }
            }

        }
    }
}
